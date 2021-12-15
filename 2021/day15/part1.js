const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let risk = [];
    for await (const line of rl) {
        risk.push(line.split("").map(r => parseInt(r, 10)));
    }

    const height = risk.length;
    const width = risk[0].length;

    // (Not actually needed since we only care about the cost, but useful for debugging)
    function reconstruct_path(cameFrom, current) {
        total_path = [current]
        while (cameFrom[current]) {
            current = cameFrom[current];
            total_path.unshift(current)
        }
            
        return total_path
    }

    function neighbours(pos) {
        const [x, y] = pos;
        const ret = [];
        if (x > 0) {
            ret.push([pos[0] - 1, pos[1]]);
        }
        if (x < width - 1) {
            ret.push([pos[0] + 1, pos[1]]);
        }
        if (y > 0) {
            ret.push([pos[0], pos[1] - 1]);
        }
        if (y < height - 1) {
            ret.push([pos[0], pos[1] + 1]);
        }
        return ret;
    }

    // Based on the pseudo-code from https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
    function findPath(start, goal) {
        openSet = [start]
        cameFrom = {}

        gScore = {}
        gScore[start] = 0

        fScore = {}
        fScore[start] = h(start)
    
        function f(pos) {
            return pos in fScore ? fScore[pos] : Infinity;
        }

        function g(pos) {
            return pos in gScore ? gScore[pos] : Infinity;
        }
        
        function h(position) {
            return (position[0]-start[0] + position[1]-start[1]);
        }

        while (openSet.length) {
            current = _.minBy(openSet, pos => f(pos))
            if (_.isEqual(current, goal)) {
                const path = reconstruct_path(cameFrom, current);
                const cost = g(goal);
                return {path, cost};
            }
            
            _.remove(openSet, pos => _.isEqual(pos, current))
            for (neighbour of neighbours(current)) {
                if (_.isEqual(neighbour, cameFrom[current])) {
                    continue;
                }

                tentative_gScore = g(current) + risk[neighbour[1]][neighbour[0]]
                previous_gScore = g(neighbour)

                if (tentative_gScore < previous_gScore) {
                    cameFrom[neighbour] = current

                    gScore[neighbour] = tentative_gScore

                    fScore[neighbour] = tentative_gScore + h(neighbour)
                    if (!openSet.find(pos => _.isEqual(pos, neighbour))) {
                        openSet.push(neighbour)
                    }
                }
            }
        }

        return false;
    }
    
    const {path, cost} = findPath([0,0], [width-1, height-1]);
    console.log(cost);

    // Debug output
    let calculatedCost = 0;
    for (let y = 0; y < height; ++y) {
        let row = "";
        for (let x = 0; x < width; ++x) {
            if (path.find(pos => _.isEqual(pos, [x, y]))) {
                row += risk[y][x];
                if (x !== 0 || y !== 0) {
                    calculatedCost += risk[y][x];
                }
            } else {
                row += ".";
            }
        }
        console.log(row);
    }
    console.log(calculatedCost);
}

processLineByLine();