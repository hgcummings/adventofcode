const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const md5 = require('md5')

async function processLineByLine() {
    const input = 'ckczppom';

    let i = 1;
    for (let hash = ""; !hash.startsWith("000000"); ++i) {
      hash = md5(input + i);
    }

    console.log(i - 1);
  }
  
  processLineByLine();