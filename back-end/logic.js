const numOfGrid = 9;
let solution = null; 
let seconds = 0;

//////////////////////////////////////////////////////////////////////////////////
//                      CREATING PUZZLE BOARD
//////////////////////////////////////////////////////////////////////////////////
//Function to find which cell is empty 
function findEmptyCell(board){
    for(let row=0; row<numOfGrid; row++){
        for(let col=0; col<numOfGrid; col++){
            if(board[row][col] === 0){
                return [row, col];
            }
            
        }
    }
    return null;
}

//Function to shuffle all the numbers in the board
function shuffle(nums){
    for(let i=nums.length-1; i>0; i--){
        const j=Math.floor(Math.random()*(i+1));
        [nums[i],nums[j]]=[nums[j],nums[i]];
    }
}

//Function to determine whether the row or column contain the number before
function isSafe(board, row, col, num){
    //check whether the number exist in columns
    for(let c=0; c<numOfGrid; c++){
        if(board[row][c]===num){
            return false;
        }
    }

    //check whether the number exist in rows
    for(let r=0; r<numOfGrid; r++){
        if(board[r][col]===num){
            return false;
        } 
    } 
    const sr = Math.floor(row/3)*3; 
    const sc = Math.floor(col/3)*3;
    //find 3x3 subgrid and check all the 9 cellss within
    for(let r=sr; r<sr+3; r++){
        for(let c=sc;c<sc+3;c++){
            if(board[r][c]===num){
                return false;
            } 
        } 
    } 
    return true;
}

//Function to solve the board
function solveBoard(board){
    const emp = findEmptyCell(board);       //search for empty grid
    if(!emp){
        return true;
    }
    const [row, col] = emp;
    const nums = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    shuffle(nums);
    for(const i of nums){
        if(isSafe(board, row, col, i)){
            board[row][col] = i;
            if(solveBoard(board)){
                return true;
            }
            board[row][col] = 0;
        }
    }
    return false;
}

//Function to generate the full board with solution 
function generateBoardWithSolution(){
    const board = [];

    //fill 0 in every cell
    for (let i = 0; i < 9; i++) {
        const row = [];
        for (let j = 0; j < 9; j++) {
            row.push(0);
        }
        board.push(row);
    }
    solveBoard(board);
    return board;
}

//Function to copy board
function copyBoard(b){ 
    return b.map(r=>r.slice()); 
}

//Function to create puzzle by removing some numbers randomly
function createPuzzle(board, numToRemove){
    const brd = copyBoard(board);
    const cells = [];
    for(let row=0; row<numOfGrid; row++){
        for(let col=0; col<numOfGrid; col++){
            cells.push([row, col]);
        }
    }
    shuffle(cells);
    let removed = 0;
    const totalToRemoved = numToRemove;
    for(const[row, col] of cells){
        if(removed >= totalToRemoved){
            break;
        }
        const backup = brd[row][col];
        brd[row][col] = 0;
        removed++;
    }
    return brd;
}

//Function to populate puzzle
function populatePuzzle(board){
    for(let row=0; row<numOfGrid; row++){
        for(let col=0; col<numOfGrid; col++){
            const inpt = inputs.children[row*9 + col];
            const val = board[row][col];

            inpt.classList.remove('given');
            inpt.classList.remove('clue');
            inpt.classList.remove('invalid');
            inpt.value = '';

            if(val !== 0){                          //if cell is not empty, marked as given
                inpt.value = String(val);
                inpt.classList.add('given');
                inpt.readOnly = true;
            }else{
                inpt.readOnly = false;              //if cell is empty, make it editable
            }
        }
    }
}

//Get the current board situation
function getCurrentBoard(){
    const b = Array.from({length:9},()=>Array(9).fill(0));
    for(let row=0; row<numOfGrid; row++){
        for(let col=0; col<numOfGrid; col++){
            const v = inputs.children[row*9 + col].value;
            b[row][col] = v? parseInt(v,10):0;
        }
    }
    return b;
}

//Check if the user has input the wrong answer
function checkPuzzle(){
    const user = getCurrentBoard();
    let allOk = true;
    // clear previous invalids
    for(const inpt of inputs.children){
        inpt.classList.remove('invalid');
    } 
    for(let row=0; row<numOfGrid; row++){
        for(let col=0; col<numOfGrid; col++){
            const val = user[row][col];
            if(val===0) { 
                allOk=false; continue; 
            }
            if(val !== solution[row][col]){
                allOk=false;
                inputs.children[row*9 + col].classList.add('invalid');
            }
        }
    }
    if(allOk){ 
        stopTimer(); 
        alert('Congratulations! You solved the puzzle in ' + formatTime(seconds) + '!');
    } else {
        alert('Wrong cells are highlighted in red.');
    }
}

module.exports = {
    // NUM_OF_GRID,
    shuffle,
    findEmptyCell,
    isSafe,
    solveBoard,
    generateBoardWithSolution,
    copyBoard,
    createPuzzle,
    checkPuzzle,
};