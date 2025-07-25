# 1. Білд-стадія — збірка проєкту
FROM node:18-alpine AS build

WORKDIR /app

# Копіюємо package.json та package-lock.json (або yarn.lock)
COPY package*.json ./

# Встановлюємо залежності
RUN npm install

# Копіюємо всі файли проєкту
COPY . .

# Збираємо проєкт для продакшену
RUN npm run build

# 2. Продукшн-стадія — запуск статичних файлів через nginx
FROM nginx:stable-alpine

# Копіюємо зібрані файли з білд-стадії
COPY --from=build /app/dist /usr/share/nginx/html

# Копіюємо свій конфіг nginx (опціонально)
# COPY nginx.conf /etc/nginx/nginx.conf

# Відкриваємо порт 80
EXPOSE 80

# Запускаємо nginx
CMD ["nginx", "-g", "daemon off;"]
