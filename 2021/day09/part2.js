const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let rows = [];
    for await (const line of rl) {
        rows.push(line.split("").map(h => parseInt(h, 10)));
    }
    
    const height = rows.length;
    const width = rows[0].length;
    
    const minima = [];
    for (let y = 0; y < height; ++y) {
        for (let x = 0; x < width; ++x) {
            const val = rows[y][x];
            if (x > 0 && rows[y][x - 1] <= val) {
                continue;
            } else if (x < width-1 && rows[y][x + 1] <= val) {
                continue;
            } else if (y > 0 && rows[y - 1][x] <= val)  {
                continue;
            } else if (y < height-1 && rows[y + 1][x] <= val) {
                continue;
            } else {
                minima.push({x: x, y: y, size: 0});
            }
        }
    }
    
    console.log(minima);
    
    for (let y = 0; y < height; ++y) {
        for (let x = 0; x < width; ++x) {
            
            const val = rows[y][x];
            if (val == 9) {
                continue;
            }
            
            let fx = x;
            let fy = y;
            
            let min;
            while (!min) {
                let neighbours = [];
                if (fx > 0) {
                    neighbours.push({x: fx - 1, y: fy});
                }
                if (fx < width-1) {
                    neighbours.push({x: fx + 1, y: fy});
                } 
                if (fy > 0)  {
                    neighbours.push({x: fx, y: fy - 1});
                } 
                if (fy < height - 1) {
                    neighbours.push({x: fx, y: fy + 1});
                }
                const lowestNeighbour = _.minBy(neighbours, n => rows[n.y][n.x]);
                
                fx = lowestNeighbour.x;
                fy = lowestNeighbour.y;
                
                min = minima.find(min => min.x == fx && min.y == fy);
            }
            
            min.size += 1;
        }
    }
    
    console.log(_(minima).sortBy(m => -m.size).take(3).reduce((p, c) => p * c.size, 1));
}

processLineByLine();