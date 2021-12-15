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
        const seen = new Set();

        cameFrom = [];
        gScore = [];
        fScore = [];

        for (let y = 0; y < height * 5; ++y) {
            gScore[y] = [];
            fScore[y] = [];
            cameFrom[y] = [];
            for (let x = 0; x < width * 5; ++x) {
                gScore[y][x] = Infinity;
                fScore[y][x] = Infinity;
            }
        }

        function f(pos) {
            return fScore[pos[1]][pos[0]];
        }

        function g(pos) {
            return gScore[pos[1]][pos[0]];
        }
        
        function h(pos) {
            return (pos[0]-start[0] + pos[1]-start[1]);
        }

        gScore[start[1]][start[0]] = 0;
        fScore[start[1]][start[0]] = h(start);

        while (openSet.length) {
            current = openSet.shift();

            if (_.isEqual(current, goal)) {
                return g(goal);
            }
            
            for (neighbour of neighbours(current)) {
                if (_.isEqual(neighbour, cameFrom[current[1],current[0]])) {
                    continue;
                }

                tentative_gScore = g(current) + r(neighbour)
                previous_gScore = g(neighbour)

                if (tentative_gScore < previous_gScore) {
                    cameFrom[neighbour[1]][neighbour[0]] = current

                    gScore[neighbour[1]][neighbour[0]] = tentative_gScore

                    fScore[neighbour[1]][neighbour[0]] = tentative_gScore + h(neighbour)
                    
                    openSet.splice(_.sortedIndexBy(openSet, neighbour, f), 0, neighbour);
                }
            }
        }

        return false;
    }
    
    const start = Date.now();
    const cost = findPath([0,0], [(width*5)-1, (height*5)-1]);
    console.log(cost);
    console.log((Date.now() - start) / 1000);
}

processLineByLine();