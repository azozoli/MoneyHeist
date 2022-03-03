FROM node:12.16-alpine as build
RUN apk update
RUN apk add gettext
WORKDIR /usr/src/app
ARG KENDO_UI_LICENSE
ENV PATH /usr/src/app/node_modules/.bin:$PATH
ADD package.json /tmp/package.json
RUN cd /tmp && npm install
RUN cp -a /tmp/node_modules /usr/src/app/
COPY ["src", "/usr/src/app/src"]
COPY ["public", "/usr/src/app/public"]
COPY ["package.json", "tsconfig.json", "/usr/src/app/"]
RUN npm rebuild node-sass
RUN npx kendo-ui-license activate
RUN npm run build
COPY ["public-config/publicConfig.js", "/usr/temp/config/publicConfig.js"]

ARG ADNET_APP_TITLE=SuppliersGas
ARG ADNET_APP_VERSION
RUN mkdir /usr/src/app/build/config/
RUN sed -i 's/${APP_PUBLIC_SETTINGS}/APP_PUBLIC_SETTINGS/g' /usr/temp/config/publicConfig.js
RUN envsubst < /usr/temp/config/publicConfig.js > /usr/src/app/build/config/publicConfig.js

FROM bitak.adnet.hr:8082/docker-nginx-unprivileged:2.0.0

COPY --from=build /usr/src/app/build /usr/share/nginx/html

ENV HTTP_INTERNAL_PORT=8080
EXPOSE 8080
