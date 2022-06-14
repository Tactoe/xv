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

$saveContent = isset($_POST["saveContent"]) ? $_POST["saveContent"] : "{}";

$stmt = $conn->prepare("INSERT INTO `saves`(`saveId`, `saveContent`) VALUES (UUID(), ?)");
$stmt->bind_param("s", $saveContent);
$stmt->execute();

$stmt = $conn->prepare("SELECT id, saveId, saveContent FROM saves WHERE saveContent = ?");
$stmt->bind_param("s", $saveContent);
$stmt->execute();
$result = $stmt->get_result();
while ($row = $result->fetch_assoc()) {
    echo $row["saveId"];
}

$conn->close();

?>