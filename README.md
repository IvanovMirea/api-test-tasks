# Тестовое задание в компанию Tele2
### Задания
###### По адресу http://testlodtask20172.azurewebsites.net/task вы можете получить строку, содержащую информацию о жителях города X. Строка содержит id, имя с фамилией и пол каждого жителя. А по адресу http://testlodtask20172.azurewebsites.net/task/{id} вы получите имя, фамилию, пол и возраст конкретного жителя, id которого можно взять из 1 запроса.

Нужно написать приложение API используя ASP.Net Core 7 и Entity Framework. В качестве БД использовать PostgreSQL или MSSQL. 

При запуске приложения сохранить список жителей и данные по ним в БД. В дальнейшем взаимодействовать с базой, чтобы получить те, или иные данные.
Нужно реализовать 2 метода API, которые будут:
### 1.	Возвращать список жителей города X. В методе необходимо реализовать фильтрацию по следующим полям: 
###### Опциональный (необязательный) фильтр по полу. Возвращать всех/только мужчин/только женщин. Если параметр не передавать, то фильтр не учитывается
###### Опциональная фильтрация по возрасту. Выводить жителей с возрастом, входящий в переданный промежуток, от x до y. Если параметр не передавать, то фильтр не учитывается
###### Так же, в методе нужна пагинация (постраничный вывод). 
###### В результате запроса нужно выводить все данные по юзеру, кроме возраста жителя.

### 2.	Возвращать конкретного жителя по переданному id.
Проект должен быть покрыт Unit-тестами, используя NUnit или XUnit. 
