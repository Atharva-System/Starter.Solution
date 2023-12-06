### STAGE 1:COPY and BUILD ###
# Defining a node image to be used as giving it an alias of "build"
# Which version of Node image to use depends on project dependencies
# This is needed to build and compile our code
# while generating the docker image
FROM public.ecr.aws/docker/library/node:18.17.1 AS build
# Create a Virtual directory inside the docker image
WORKDIR /src/angular
# Copy files to virtual directory
# COPY package.json package-lock.json ./
# Run command in Virtual directory
RUN npm cache clean --force
# Copy files from local machine to virtual directory in docker image
COPY ["src/UI/Angular", "."]
RUN npm install -g npm@9.6.7
RUN npm install
RUN npm run build

### # STAGE 2: SERVE THE ANGULAR APPLICATION ###
# Defining nginx image to be used
FROM public.ecr.aws/nginx/nginx:latest AS ngi
# Copying compiled code and nginx config to different folder
# NOTE: This path may change according to your project's output folder
COPY --from=build /src/angular/dist/angular.starter /usr/share/nginx/html
COPY ["src/UI/Angular/conf/default.conf", "/etc/nginx/conf.d/default.conf"]
# Exposing a port, here it means that inside the container
# the app will be using Port 80 while running
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
