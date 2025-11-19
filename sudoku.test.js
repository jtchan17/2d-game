const { 
    isSafe, 
    solveBoard, 
    generateBoardWithSolution, 
    createPuzzle, 
    copyBoard,
} = require('./back-end/logic'); // your JS file exported as a module


test('copyBoard creates a deep copy', () => {
    const board = [
        [1,0,0],
        [0,2,0],
        [0,0,3]
    ];
    const copy = copyBoard(board);
    expect(copy).toEqual(board);
    copy[0][0] = 9;
    expect(board[0][0]).toBe(1);  // original not modified
});

test('isSafe returns true for safe number placement', () => {
    const board = [
        [5,3,0,0,7,0,0,0,0],
        [6,0,0,1,9,5,0,0,0],
        [0,9,8,0,0,0,0,6,0],
        [8,0,0,0,6,0,0,0,3],
        [4,0,0,8,0,3,0,0,1],
        [7,0,0,0,2,0,0,0,6],
        [0,6,0,0,0,0,2,8,0],
        [0,0,0,4,1,9,0,0,5],
        [0,0,0,0,8,0,0,7,9]
    ];
    expect(isSafe(board, 0, 2, 1)).toBe(true);
    expect(isSafe(board, 0, 2, 5)).toBe(false); // 5 exists in same row
});

test('solveBoard solves a partially filled board', () => {
    const board = [
        [5,3,0,0,7,0,0,0,0],
        [6,0,0,1,9,5,0,0,0],
        [0,9,8,0,0,0,0,6,0],
        [8,0,0,0,6,0,0,0,3],
        [4,0,0,8,0,3,0,0,1],
        [7,0,0,0,2,0,0,0,6],
        [0,6,0,0,0,0,2,8,0],
        [0,0,0,4,1,9,0,0,5],
        [0,0,0,0,8,0,0,7,9]
    ];
    expect(solveBoard(board)).toBe(true);
    // All cells should be non-zero after solving
    for(let r=0; r<9; r++){
        for(let c=0; c<9; c++){
            expect(board[r][c]).not.toBe(0);
        }
    }
});

test('generateBoardWithSolution returns a full board', () => {
    const board = generateBoardWithSolution();
    expect(board.length).toBe(9);
    for(const row of board){
        expect(row.length).toBe(9);
        for(const cell of row){
            expect(cell).toBeGreaterThanOrEqual(1);
            expect(cell).toBeLessThanOrEqual(9);
        }
    }
});

test('createPuzzle removes correct number of cells', () => {
    const board = generateBoardWithSolution();
    const numToRemove = 40;
    const puzzle = createPuzzle(board, numToRemove);
    let countEmpty = 0;
    for(const row of puzzle){
        for(const cell of row){
            if(cell === 0) countEmpty++;
        }
    }
    expect(countEmpty).toBe(numToRemove);
});
