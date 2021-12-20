const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let algo;
    let rawImage = [];
    for await (const line of rl) {
        if (!algo) {
            algo = line.split("").map(c => c === "#" ? 1 : 0);
        } else if (line.length) {
            rawImage.push(line.split("").map(c => c === "#" ? 1 : 0));
        }
    }
    rawImage.field = 0;

    function getPixel(imageIn, x, y) {
        let index = "";
        for (let iy = y - 1; iy <= (y + 1); ++iy) {
            for (let ix = x - 1; ix <= (x + 1); ++ix) {
                if (iy >= 0 && iy < imageIn.length && ix >=0 && ix < imageIn[iy].length) {
                    index += imageIn[iy][ix];
                } else {
                    index += imageIn.field;
                }
            }
        }
        return algo[parseInt(index, 2)];
    }

    function enhance(imageIn) {
        const pad = 3;
        let imageOut = [];
        for (let y = -pad; y < (imageIn.length + pad); ++y) {
            imageOut[y + pad] = []; 
            for (let x = -pad; x < (imageIn.length + pad); ++x) {
                imageOut[y + pad][x + pad] = getPixel(imageIn, x, y);
            }
        }
        imageOut.field = getPixel(imageIn, -pad, -pad);
        return imageOut;
    }
    
    let result = rawImage;
    for (let step = 0; step < 50; ++step) {
        result = enhance(result);
    }
    for (let y = 0; y < result.length; ++y) {
        console.log(result[y].map(x => x === 1 ? '#' : '.').join(""));
    }
    console.log(_.sumBy(result, row => _.sum(row))); 
}

processLineByLine();