const {performance} = require('perf_hooks');
const ArrayKeyedMap = require('array-keyed-map');
const { MinPriorityQueue } = require('@datastructures-js/priority-queue');

const startTime = performance.now();

const chain = new ArrayKeyedMap();
const g = new ArrayKeyedMap();
const h = new ArrayKeyedMap();

const bossInitialHp = 51;
const bossDamage = 9;

// [bossHp,playerHp,playerMana,shieldT,poisonT,rechargeT];
const initialState = [bossInitialHp,49,500,0,0,0];
g.set(initialState, 0);
chain.set(initialState, "");

const queue = new MinPriorityQueue(e => e.f);
queue.enqueue({ state: initialState, priority: f(initialState) })

function f(state) {
    if (!h.has(state)) {
        h.set(state,state[0] / (173 / 18));
    }
    return g.get(state) + h.get(state);
}

const options = [
    {   // Magic Missile costs 53 mana. It instantly does 4 damage.
        applicable: ([_0,_1,playerMana]) => playerMana >= 53,
        apply: ([bossHp,playerHp,playerMana,shieldT,poisonT,rechargeT]) =>
                [bossHp-4,playerHp,playerMana-53,shieldT,poisonT,rechargeT]
    },
    {   // Drain costs 73 mana. It instantly does 2 damage and heals you for 2 hit points.
        applicable: ([_0,_1,playerMana]) => playerMana >= 73,
        apply: ([bossHp,playerHp,playerMana,shieldT,poisonT,rechargeT]) =>
                [bossHp-2,playerHp+2,playerMana-73,shieldT,poisonT,rechargeT]
    },
    {   // Shield costs 113 mana. It starts an effect that lasts for 6 turns. While it is active, your armor is increased by 7.
        applicable: ([_0,_1,playerMana,shieldT]) => playerMana >= 113 && shieldT === 0,
        apply: ([bossHp,playerHp,playerMana,_,poisonT,rechargeT]) =>
                [bossHp,playerHp,playerMana-113,6,poisonT,rechargeT]
    },
    {   // Poison costs 173 mana. It starts an effect that lasts for 6 turns. At the start of each turn while it is active, it deals the boss 3 damage.
        applicable: ([_0,_1,playerMana,_2,poisonT]) => playerMana >= 173 && poisonT === 0,
        apply: ([bossHp,playerHp,playerMana,shieldT,_,rechargeT]) =>
                [bossHp,playerHp,playerMana-173,shieldT,6,rechargeT]
    },
    {   // Recharge costs 229 mana. It starts an effect that lasts for 5 turns. At the start of each turn while it is active, it gives you 101 new mana.
        applicable: ([_0,_1,playerMana,_2,_3,rechargeT]) => playerMana >= 229 && rechargeT === 0,
        apply: ([bossHp,playerHp,playerMana,shieldT,poisonT,_]) =>
                [bossHp,playerHp,playerMana-229,shieldT,poisonT,5]
    }
]

function applyEffects(state) {
    let [bossHp,playerHp,playerMana,shieldT,poisonT,rechargeT] = state;
    let playerArmour = 0;
    if (shieldT > 0) {
        shieldT -= 1;
        playerArmour = 7;
    }
    if (poisonT > 0) {
        poisonT -= 1;
        bossHp -= 3;
    }
    if (rechargeT > 0) {
        rechargeT -= 1;
        playerMana += 101;
    }
    return [[bossHp,playerHp,playerMana,shieldT,poisonT,rechargeT],playerArmour];
}

let best = Infinity;
while(!queue.isEmpty()) {
    let startOfRound = queue.dequeue().state;
    
    if (startOfRound[0] <= 0) {
        if (g.get(state) < best) {
            console.log(chain.get(state));
            console.log(g.get(state));
        }
        continue;
    }

    let [startOfPlayerTurn] = applyEffects(startOfRound);

    for (const option of options) {
        if (option.applicable(startOfPlayerTurn)) {
            const endOfPlayerTurn = option.apply(startOfPlayerTurn);
            let manaSpent = startOfPlayerTurn[2] - endOfPlayerTurn[2];
            let newG = g.get(startOfRound) + manaSpent;

            const [startOfMonsterTurn,playerArmour] = applyEffects(endOfPlayerTurn);
            let [bossHp,playerHp,playerMana,shieldT,poisonT,rechargeT] = startOfMonsterTurn;
            let newChain = chain.get(startOfRound) + `\n (${startOfPlayerTurn}) ${manaSpent} (${startOfMonsterTurn})`;
           
            if ((bossHp <= 0)) {
                if (newG < best) {
                    best = newG;
                    console.log(newChain);
                    console.log(best);
                }
                continue;
            }

            const damage = Math.max(1, bossDamage - playerArmour);
            playerHp -= damage;
            playerHp -= 1;
            if (playerHp <= 0) {
                continue;
            }

            const endOfMonsterTurn = [bossHp,playerHp,playerMana,shieldT,poisonT,rechargeT];
            if ((!g.has(endOfMonsterTurn)) || newG < g.get(endOfMonsterTurn)) {
                g.set(endOfMonsterTurn, newG);
                chain.set(endOfMonsterTurn, newChain);

                queue.enqueue({
                    state: endOfMonsterTurn,
                    priority: f(endOfMonsterTurn)
                });
            }
        }
    }
}

console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
