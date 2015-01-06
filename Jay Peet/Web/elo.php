<?php
//Get Current Players Elo before calculating the new elo based on win loss.
echo 'Start:';
CalculateElo("Jay","Andrew","Jay",1);


function GetPlayersElo($player){
	return 1;
}

//$player1 and $player2 would be the users username or ID.

function CalculateElo($player1,$player2,$outcome,$K) {
    echo "Starting Calculation";
	
	//Grab Current Elo for players
	$P1 = 1;
	$P2 = 1;
	
	echo("<br>");
	echo("Player 1 Start : " . $P1);
	echo("<br>");
	echo("Player 2 Start : " . $P2);
	echo("<br>");
	echo("<br>");
	
	
	
	//Work Out expected score.
	$P1E = $P1 / ($P1 + $P2);
	$P2E = $P2 / ($P1 + $P2);
	
	//Set outcome. Win(1), Lose(0) or Draw(0.5).
	switch ($outcome) {
    case $player1:
        $P1O = 1;
        $P2O = 0;
		break;
    case $player2:
        $P1O = 0;
		$P2O = 1;
        break;
    case "draw":
        $P1O = 0.5;
        $P2O = 0.5;
		break;
	}
	echo("Player 1 Wins");
	//Calculate the players new ELO base on that.
	$P1 = $P1 + $K * ($P1O - $P1E);
	$P2 = $P2 + $K * ($P2O - $P2E);
	
	echo("<br>");
	echo("<br>");
	echo("Player 1 End : " . $P1);
	echo("<br>");
	echo("Player 2 End : " . $P2);
}
?>

