const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let dots = [];
    let folds = [];
    for await (const line of rl) {
        if (line === "") {
            continue;
        }
        if (line.startsWith("fold")) {
            [axis, num] = line.split("=");
            folds.push({
                axis: axis[axis.length-1],
                offset: parseInt(num)
            });
        } else {
            const [x, y] = line.split(",");
            dots.push({x: parseInt(x, 10), y: parseInt(y, 10)});
        }
    }

    for (const fold of folds) {
        for (let i = dots.length - 1; i >= 0; --i) {
            const dot = dots[i];
            if (dot[fold.axis] === fold.offset) {
                dots.splice(i, 1);
            } else if (dot[fold.axis] > fold.offset) {
                dot[fold.axis] = (2 * fold.offset) - dot[fold.axis];
            }
        }

        dots = _.uniqWith(dots, _.isEqual);

        break;
    }
    
    console.log(dots.length);
}

processLineByLine();