# Stage 1: Build the React app using Node v22
FROM node:22-alpine AS build
WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy all project files and build the app
COPY . .
RUN npm run build

# Stage 2: Serve the built app with Nginx
FROM nginx:alpine

# Copy build assets from the previous stage to Nginx's directory
COPY --from=build /app/build /usr/share/nginx/html

# Copy the env-config.js.template to the html folder
COPY public/env-config.js.template /usr/share/nginx/html/

# Copy the entrypoint script
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

# Expose port 80 (Nginx default)
EXPOSE 80

# Use the entrypoint script to perform runtime substitution before starting Nginx
ENTRYPOINT ["/entrypoint.sh"]
