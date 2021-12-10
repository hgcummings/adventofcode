const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });

    const badPairs = ["ab", "cd", "pq", "xy"];
    const vowels = ["a", "e", "i", "o", "u"];
    function isNice(str) {
        let vowelCount = 0;
        let repeatChar = false;

        let prev = "";
        for (const char of str) {
            if (badPairs.includes(prev + char)) {
                return false;
            }
            if (vowels.includes(char)) {
                vowelCount += 1;
            }
            if (prev == char) {
                repeatChar = true;
            }
            prev = char;
        }

        return vowelCount >= 3 && repeatChar;
    }
  
    let count = 0;
    for await (const line of rl) {
        if (isNice(line)) {
            count += 1;
        }
    }

    console.log(count);
  }
  
  processLineByLine();