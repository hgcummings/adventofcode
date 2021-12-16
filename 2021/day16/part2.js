const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let inputHex;
    for await (const line of rl) {
        inputHex = line;
        break;
    }

    let input = "";
    for (let digit of inputHex) {
        input += parseInt(digit, 16).toString(2).padStart(4, "0");
    }

    function takeNext(str, count) {
        return [str.substring(0, count), str.substring(count)];
    }

    function takePacket(str) {
        const newPacket = {};
        let version, type, lengthType, length;
        [version, str] = takeNext(str, 3);
        [type, str] = takeNext(str, 3);

        newPacket.version = parseInt(version, 2);
        newPacket.type = parseInt(type, 2);

        if (newPacket.type === 4) {
            let value = "";
            while (true) {
                let nextChunk;
                [nextChunk, str] = takeNext(str, 5);
                value += nextChunk.substring(1);

                if (nextChunk[0] === "0") {
                    break;
                }
            }
            newPacket.value = parseInt(value, 2);
        } else {
            [lengthType, str] = takeNext(str, 1);
            [length, str] = takeNext(str, lengthType === "1" ? 11 : 15);

            newPacket.length = parseInt(length, 2);
            newPacket.subPackets = [];

            if (lengthType === "0") {
                const startLength = str.length;
                while (str.length > startLength - newPacket.length) {
                    let subPacket;
                    [subPacket, str] = takePacket(str);
                    newPacket.subPackets.push(subPacket);
                }
            } else {
                while (newPacket.subPackets.length < newPacket.length) {
                    let subPacket;
                    [subPacket, str] = takePacket(str);
                    newPacket.subPackets.push(subPacket);
                }
            }

            switch (newPacket.type) {
                case 0:
                    newPacket.value = _.sumBy(newPacket.subPackets, p => p.value);
                    break;
                case 1:
                    newPacket.value = _.reduce(newPacket.subPackets, (curr, next) => curr * next.value, 1);
                    break;
                case 2:
                    newPacket.value = _.min(newPacket.subPackets.map(p => p.value));
                    break;
                case 3:
                    newPacket.value = _.max(newPacket.subPackets.map(p => p.value));
                    break;
                case 5:
                    newPacket.value = newPacket.subPackets[0].value > newPacket.subPackets[1].value ? 1 : 0;
                    break;
                case 6:
                    newPacket.value = newPacket.subPackets[0].value < newPacket.subPackets[1].value ? 1 : 0;
                    break;
                case 7:
                    newPacket.value = newPacket.subPackets[0].value === newPacket.subPackets[1].value ? 1 : 0;
                    break;                    
            }

            if (!isFinite(newPacket.value)) {
                console.log(newPacket);
                console.log(newPacket.subPackets);
                process.exit();
            }
        }

        return [newPacket, str];
    }

    let packets = [];
    while (input.length) {
        let newPacket;
        [newPacket, input] = takePacket(input);
        packets.push(newPacket);
        if (parseInt(input, 2) === 0) {
            break;
        }
    }

    console.log(packets[0].value);
}

processLineByLine();