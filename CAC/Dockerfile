# Stage 0, "build-stage", based on Node.js, to build and compile the frontend
FROM node:14.18.1-alpine as build
WORKDIR /app
COPY package*.json /app/
RUN npm install @angular/core@12.2.4 --save 
COPY ./ /app/
ARG configuration=production
RUN npm run build -- --output-path=./dist/out --configuration $configuration
# Stage 1, based on Nginx, to have only the compiled app, ready for production with Nginx

FROM nginx:1.15-alpine

COPY --from=build /app/dist/out/ /usr/share/nginx/html

# Copy the default nginx.conf provided by alpine
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80