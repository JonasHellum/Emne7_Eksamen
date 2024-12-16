DROP DATABASE IF EXISTS gokstad_athletics;
CREATE DATABASE gokstad_athletics;
use gokstad_athletics;

GRANT ALL privileges ON gokstad_athletics.* TO 'ga-app'@'%';
GRANT ALL privileges ON gokstad_athletics.* TO 'ga-app'@'localhost';