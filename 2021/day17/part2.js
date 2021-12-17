const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

//target area: x=20..30, y=-10..-5
// const xmin = 20;
// const xmax = 30;
// const ymin = -10;
// const ymax = -5;

// target area: x=128..160, y=-142..-88
const xmin = 128;
const xmax = 160;
const ymin = -142;
const ymax = -88;

let isValid = true;
let yinit, ypeak;
let count = 0;
for (yinit = ymin; yinit < -1 * (ymin - 1); ++yinit) {
    isValid = false;
    ypeak = -Infinity;
    for (let xinit = 0; xinit < xmax + 1; ++xinit) {
        let hit = false, missed = false;
        let x = 0, y = 0;
        let dx = xinit, dy = yinit;
        while (!(hit || missed)) {
            x += dx;
            y += dy;
            dx -= Math.sign(dx);
            dy -= 1;

            if (y > ypeak) {
                ypeak = y;
            }

            if (x > xmax || y < ymin) {
                missed = true;
            } else if (x >= xmin && y <= ymax) {
                hit = true;
            }
        }
        if (hit) {
            isValid = true;
            count += 1;
        }
    }
}

console.log(count);