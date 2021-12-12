const fs = require('fs');
const readline = require('readline');

async function processLineByLine() {
    const fileStream = fs.createReadStream('input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let pos = 0;
    let depth = 0;
    let aim = 0;
    for await (const line of rl) {
        let [dir, mag] = line.split(' ');
        mag = parseInt(mag, 10);
        switch (dir) {
            case "forward":
                pos += mag;
                depth += aim * mag;
            break;
                case "down":
                aim += mag;
            break;
                case "up":
                aim -= mag;
                break;
            default:
                console.error($`invalid direction ${dir}`);
                break;
        }
    }
    
    console.log(pos, depth, pos * depth);
}

processLineByLine();