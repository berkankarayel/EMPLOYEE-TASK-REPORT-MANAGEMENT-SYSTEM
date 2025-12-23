# 1️⃣ Build stage
FROM node:20-alpine AS build

WORKDIR /app

# Package files sadece - cache optimization
COPY package*.json ./

# Dependencies install (devDependencies dahil - build için gerekli)
RUN npm ci

# Source code kopyala
COPY . .

# Build arguments for API URL
ARG VITE_API_URL=http://localhost:8080/api
ENV VITE_API_URL=$VITE_API_URL

# Production build
RUN npm run build

# 2️⃣ Production stage
FROM nginx:alpine

# Custom nginx config (SPA routing için)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Build artifacts kopyala
COPY --from=build /app/dist /usr/share/nginx/html

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --quiet --tries=1 --spider http://localhost:80/ || exit 1

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]