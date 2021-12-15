const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let input;
    for await (const line of rl) {
        input = line;
        break;
    }

    const base26Chars = ['0','1','2','3','4','5','6','7','8','9','a','b','c','e','f','g','h','i','j','k','l','m','n','o','p']; 
    const alphaChars =  ['a','b','c','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'];

    function toAlpha(base26String) {
        let alphaString = base26String;
        for (let i = base26Chars.length-1; i >= 0; --i) {
            let regex = new RegExp(base26Chars[i], "g");
            alphaString = alphaString.replace(regex, alphaChars[i]);
        }
        return alphaString;
    }

    function toBase26(alphaString) {
        let base26String = alphaString;
        for (let i = 0; i < alphaChars.length; ++i) {
            let regex = new RegExp(alphaChars[i], "g");
            base26String = base26String.replace(regex, base26Chars[i]);
        }
        return base26String;
    }

    const badChars = ['i', 'o', 'l'].map(toBase26);
    function isValid(number) {
        const base26String = new Number(number).toString(26);

        let foundStraight = false;
        let foundPairs = [];
        for (let i = 0; i < base26String.length; ++i) {
            const char = base26String[i];
            if (badChars.includes(char)) {
                return false;
            }
            if (!foundStraight && i > 2) {
                const charValue = parseInt(char, 26);
                if (parseInt(base26String[i-1], 26) === charValue - 1 &&
                    parseInt(base26String[i-2], 26) === charValue - 2) {
                        foundStraight = true;
                    }
            }
            if (foundPairs.length < 2) {
                if (foundPairs.length === 0 || i > foundPairs[0] + 1) {
                    if (char === base26String[i-1]) {
                        foundPairs.push(i);
                    }
                }
            }
        }

        return foundStraight && foundPairs.length >= 2;
    }


    const initialBase26 = toBase26(input);

    let number;
    for (number = parseInt(initialBase26, 26); !isValid(number); ++number) {
        
    }

    console.log(toAlpha(number.toString(26)));
}

processLineByLine();