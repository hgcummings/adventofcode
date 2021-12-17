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

const yinit = (-1 * ymin) - 1;
const ypeak = (yinit + 1) * (yinit / 2);
console.log(yinit, ypeak);