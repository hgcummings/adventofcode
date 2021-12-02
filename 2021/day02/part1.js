const fs = require('fs');
const readline = require('readline');

async function processLineByLine() {
    const fileStream = fs.createReadStream('input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let length = 0;
    let depth = 0;
    for await (const line of rl) {
        let [dir, mag] = line.split(' ');
        mag = parseInt(mag, 10);
        switch (dir) {
            case "forward":
                length += mag;
                break;
            case "down":
                depth += mag;
                break;
            case "up":
                depth -= mag;
                break;
            default:
                console.error($`invalid direction ${dir}`);
                break;
        }
    }

    console.log(length, depth, length * depth);
  }
  
  processLineByLine();