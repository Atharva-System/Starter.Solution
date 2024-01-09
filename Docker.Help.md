# Starter

# Run App in Local environment
	- Open Develeoper Poweshell with root path of 'docker-compose.yml'  
		OR
	- After opening Solution, right click on docker-compose, Click on 'Open in Terminal'
	
# Docker Commands
	- To Build App :  docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build
	  or
	- To Up Docker Images : docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
	- To down Docker Images :docker-compose -f docker-compose.yml -f docker-compose.override.yml down

	- docker --help
	- List Containers : docker ps
	- List docker images : docker image ls

 ## Run Individual (at root path of repo)
    - Blazor APP
        - build image : docker build -t starter.blazor -f src/UI/Blazor/Dockerfile ./
        - run contaimer : docker run -e apiURL=http://localhost:10001  --name starter.blazor starter.blazor
  