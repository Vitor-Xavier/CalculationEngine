FROM nginx:alpine
RUN rm /etc/nginx/nginx.conf /etc/nginx/conf.d/default.conf
COPY src/nginx/nginx.conf /etc/nginx
COPY src/nginx/content/index.html /usr/share/nginx/html/index.html
VOLUME /var/log/nginx/log