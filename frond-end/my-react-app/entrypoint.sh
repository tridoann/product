#!/bin/sh
# Substitute environment variables in the template file
envsubst < /usr/share/nginx/html/env-config.js.template > /usr/share/nginx/html/env-config.js
# Launch Nginx
exec nginx -g 'daemon off;'