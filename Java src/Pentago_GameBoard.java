package minmax;

public class Pentago_GameBoard {

	/* indexes in the game board
	 * 0  1  2 | 3  4  5
	 * 6  7  8 | 9  10 11
	 * 12 13 14| 15 16 17
	 * -------------------
	 * 18 19 20| 21 22 23
	 * 24 25 26| 27 28 29
	 * 30 31 32| 33 34 35 
	 * */
	
	public int[] board; /*faster 2 read if needed*/
	private boolean player_turn;
	boolean get_player_turn(){return player_turn;}
	void switch_player_turn(){player_turn = !player_turn;}
	
	public static final int is_empty = 0;
	public static final int has_white = 1;
	public static final int has_black = 2;
	public static final boolean whites_turn = false;
	public static final boolean blacks_turn = true;
	
	Pentago_GameBoard()
	{
		board = new int[36];
		for(int i = board.length-1; i>=0; --i) board[i] = 0;
		player_turn = whites_turn;
	}
	
	Pentago_GameBoard(int[] board, boolean turn)
	{
		this.board = board;//may need 2 clone here ???
		player_turn = turn;
	}
	
static int board_postion_to_index(int x, int y , int square) throws Exception{
	
	//Pre contitions -------------------------------------------------
	if (x<0 | x>2) throw new Exception("Invalid x");
	if (y<0 | y>2) throw new Exception("Invalid y");
	if (square<0 | square>3) throw new Exception("Invalid Square");
	
	//Method body ----------------------------------------------------
	int index=0;
	
	switch (square) {
	case 0:
		break;
	case 1:		index=3;	
		break;
	case 2:		index=18;	
		break;
	case 3:		index=21;	
		break;
	default:
		break;
	}
	
	return index + x + y*6;
}


void print_board(){
	
	System.out.println(" --------------- "); 
	int index=0;
	for(int i = 0; i<12;++i )
	{
		System.out.print("| "); 
		while(index<i*3+3) 
		{
			System.out.print(board[index] + " "); 
			index++;
		}
		if(i % 2 == 1) System.out.println("| "); 
		if(i==5) System.out.println("|---------------|"); 
	}
	System.out.println(" --------------- "); 
}

}