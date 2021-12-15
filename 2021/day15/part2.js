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

    function r(pos) {
        const base = risk[pos[1] % height][pos[0] % width];
        let incr = base + Math.floor(pos[1] / height) + Math.floor(pos[0] / width);
        return incr > 9 ? (incr - 9) : incr;
    }

    function neighbours(pos) {
        const [x, y] = pos;
        const ret = [];
        if (x > 0) {
            ret.push([pos[0] - 1, pos[1]]);
        }
        if (x < (width*5) - 1) {
            ret.push([pos[0] + 1, pos[1]]);
        }
        if (y > 0) {
            ret.push([pos[0], pos[1] - 1]);
        }
        if (y < (height*5) - 1) {
            ret.push([pos[0], pos[1] + 1]);
        }
        return ret;
    }


     // Based on the pseudo-code from https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode
     function findPath(start, goal) {
        const openSet = [start];

        cameFrom = [];
        const pathCosts = [];

        for (let y = 0; y < height * 5; ++y) {
            pathCosts[y] = [];
            cameFrom[y] = [];
            for (let x = 0; x < width * 5; ++x) {
                pathCosts[y][x] = Infinity;
            }
        }

        function pathCostOf(pos) {
            return pathCosts[pos[1]][pos[0]];
        }
        
        pathCosts[start[1]][start[0]] = 0;

        while (openSet.length) {
            current = openSet.shift();

            if (_.isEqual(current, goal)) {
                return pathCostOf(goal);
            }
            
            for (neighbour of neighbours(current)) {
                if (_.isEqual(neighbour, cameFrom[current[1],current[0]])) {
                    continue;
                }

                let newPathCost = pathCostOf(current) + r(neighbour);
                let prevPathCost = pathCostOf(neighbour);

                if (newPathCost < prevPathCost) {
                    cameFrom[neighbour[1]][neighbour[0]] = current;

                    pathCosts[neighbour[1]][neighbour[0]] = newPathCost;

                    openSet.splice(_.sortedIndexBy(openSet, neighbour, pathCostOf), 0, neighbour);
                }
            }
        }

        return false;
    }
    
    const cost = findPath([0,0], [(width*5)-1, (height*5)-1]);
    console.log(cost);
}

processLineByLine();