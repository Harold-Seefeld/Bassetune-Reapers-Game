<?php
//Early inventory script. Missing lots of bits and bobs.
$username = "Test";


$mode = $_GET['mode'];

//Check variables to sort out there values.
if(!isset($_GET["username"])){
$username = null;
}
elseif(isset($_GET["username"])){
$username = $_GET["username"];
}

if(!isset($_GET["bought_item"])){
$bought_item = null;
}
elseif(isset($_GET["bought_item"])){
$bought_item = $_GET["bought_item"];
}

if(!isset($_GET["mode"])){
$mode = "error";
}
elseif(isset($_GET["mode"])){
$mode = $_GET["mode"];
}




if($mode == "view" && $username!=null){
	//Connect to DB and retrieve items.
	$connection = mysqli_connect("localhost","test","testpassword","test");
	if (mysqli_connect_errno())
	{
		echo "DatabaseFail";
		exit;
	}
	
	//Read in users inventory from the XML file.
	$filename = $username . ".xml";
	
	$userInventory = simplexml_load_file($filename);
	
	foreach ($userInventory as $item){
        $title=$item->name;
        echo $title . "<br>";
    }
}
elseif($mode == "add" && $username!=null){
	//Check to see if the item which was purchased is in the database. If so, we add it to the users inventory.
	

	
}

//echo "Done";
?>