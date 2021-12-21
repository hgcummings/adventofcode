
const scores = [0,0];
const positions = [1,0];

function* d100() {
    let i = 0;
    while (true) {
        yield (i % 100) + 1;
        i += 1;
    }
}

let rolls = 0;
const die = d100();
while ((scores[0] < 1000) && (scores[1] < 1000)) {
    for (let i = 0; i < scores.length; ++i){
        const movement = die.next().value + die.next().value + die.next().value;
        rolls += 3;
        positions[i] = (positions[i] + movement) % 10;
        scores[i] += (positions[i] + 1);
    }
}

console.log(rolls);

console.log(rolls * scores[0]);

console.log(rolls * scores[1]);