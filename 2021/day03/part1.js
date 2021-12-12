const fs = require('fs');
const readline = require('readline');

async function processLineByLine() {
    const fileStream = fs.createReadStream('input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let count = 0;
    let epsilon = [0,0,0,0,0,0,0,0,0,0,0,0];
    let gamma = [0,0,0,0,0,0,0,0,0,0,0,0];
    for await (const line of rl) {
        line.split('').forEach((d, i) => {
            if (d === '1') {
                epsilon[i] += 1;
            } else {
                gamma[i] += 1;
            }
        });
        count += 1;
    }
    
    epsilon = epsilon.map(x => x >= count / 2 ? '1' : '0').join('');
    gamma = gamma.map(x => x >= count / 2 ? '1' : '0').join('');
    
    epsilon = parseInt(epsilon, 2);
    gamma = parseInt(gamma, 2);
    
    console.log(epsilon, gamma, epsilon * gamma);
}

processLineByLine();