const fs = require('fs');
const readline = require('readline');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let lines = [];
    for await (const line of rl) {
        lines.push(line);
    }

    console.log(increases);
  }
  
  processLineByLine();