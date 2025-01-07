DROP DATABASE IF EXISTS gokstad_athletics;
CREATE DATABASE gokstad_athletics;
use gokstad_athletics;

CREATE USER IF NOT EXISTS 'ga-app'@'localhost' IDENTIFIED BY 'ga-5ecret-%';
CREATE USER IF NOT EXISTS 'ga-app'@'%' IDENTIFIED BY 'ga-5ecret-%';

GRANT ALL privileges ON gokstad_athletics.* TO 'ga-app'@'%';
GRANT ALL privileges ON gokstad_athletics.* TO 'ga-app'@'localhost';