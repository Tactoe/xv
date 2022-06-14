<?php

$db_host = "localhost";

getenv('MYSQL_DBHOST') ? $db_host=getenv('MYSQL_DBHOST') : $db_host="localhost";
getenv('MYSQL_DBPORT') ? $db_port=getenv('MYSQL_DBPORT') : $db_port="3306";
getenv('MYSQL_DBUSER') ? $db_user=getenv('MYSQL_DBUSER') : $db_user="root";
getenv('MYSQL_DBPASS') ? $db_pass=getenv('MYSQL_DBPASS') : $db_pass="";
getenv('MYSQL_DBNAME') ? $db_name=getenv('MYSQL_DBNAME') : $db_name="saves";

if (strlen( $db_name ) === 0)
  $conn = new mysqli("$db_host:$db_port", $db_user, $db_pass);
else 
  $conn = new mysqli("$db_host:$db_port", $db_user, $db_pass, $db_name);

$idToGet = isset($_POST["saveId"]) ? $_POST["saveId"] : "test";

$stmt = $conn->prepare("SELECT id, saveId, saveContent FROM saves WHERE saveId = ?");
$stmt->bind_param("s", $idToGet);
$stmt->execute();
$result = $stmt->get_result();
while ($row = $result->fetch_assoc()) {
    echo $row["saveContent"];
}

$conn->close();

?>