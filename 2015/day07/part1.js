const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let wires = {};

    function parseValue(v) {
        if (isFinite(parseInt(v, 10))) {
            return parseInt(v, 10);
        } else {
            return wires[v];
        }
    }

    function parseInput(i) {
        if (i.length === 1) {
            return parseValue(i[0]);
        } else if (i.length === 2) {
            const v = parseValue(i[1]);
            if (v === undefined) {
                return undefined;
            }
            return v ^ 65535;
        } else if (i.length === 3) {
            let [a, op, b] = i;
            a = parseValue(a);
            b = parseValue(b);
            if (a === undefined || b === undefined) {
                return undefined;
            }
            switch(op) {
                case "AND":
                    return a & b;
                case "OR":
                    return a | b;
                case "LSHIFT":
                    return a << b;
                case "RSHIFT":
                    return a >> b;
                default:
                    console.log("Unknown operator", op);
                    break;
            }
        }
    }

    const lines = [];
    for await (const line of rl) {
        lines.push(line);
    }

    while (lines.length) {
        for (let i = lines.length - 1; i >= 0; --i) {
            let [input, target] = lines[i].split(" -> ");
            const output = parseInput(input.split(" "));
            if (output !== undefined) {
                wires[target] = output;
                lines.splice(i, 1);
            }
        }
    }
    
    console.log(wires["a"]);
}

processLineByLine();