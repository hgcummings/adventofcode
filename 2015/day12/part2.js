const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let data;
    for await (const line of rl) {
        data = JSON.parse(line);
        break;
    }

    function getSum(node) {
        if (typeof node === "object") {
            if (Array.isArray(node)) {
                return _.sumBy(node, getSum);
            } else {
                let sum = 0;
                for (let key in node) {
                    if (node[key] === "red") {
                        return 0;
                    }
                    sum += getSum(node[key]);
                }
                return sum;
            }
        } else if (typeof(node) === "number") {
            return node;
        } else {
            return 0;
        }
    } 
    
    console.log(getSum(data));
}

processLineByLine();