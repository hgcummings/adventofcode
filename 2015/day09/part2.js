const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let edges = [];
    for await (const line of rl) {
        const [nodes, length] = line.split(" = ");
        edges.push({
            nodes: nodes.split(" to "),
            length: parseInt(length, 10)
        });
    }

    let allNodes = _(edges).flatMap(e => e.nodes).uniq().value();
    let paths = allNodes.map(n => ({ nodes: [n], length: 0 }));
    let completedPaths = [];

    while (paths.length) {
        foundPaths = false;

        const newPaths = [];
        for (const path of paths) {
            for (const edge of edges) {
                if (edge.nodes.includes(path.nodes[0])) {
                    const newNode = edge.nodes[1-edge.nodes.indexOf(path.nodes[0])];
                    if (!path.nodes.includes(newNode)) {
                        const newPath = {
                            nodes: [newNode].concat(path.nodes),
                            length: path.length + edge.length
                        };
                        if (newPath.nodes.length === allNodes.length) {
                            completedPaths.push(newPath)
                        } else {
                            newPaths.push(newPath);
                        }
                    }
                }
            }
        }

        paths = newPaths;
    }

    console.log(_.max(completedPaths.map(path => path.length)));
}

processLineByLine();