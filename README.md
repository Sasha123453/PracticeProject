# PracticeProject
Ссылка на файл бд с небольшим количество записей: https://drive.google.com/file/d/1RaqmRhkHTuQAmAFhkmYwRt1d3ObPt-bs/view?usp=sharing
Сделать нынешнего пользователя администратором можно добавив следющий код в начало метода ShowResourcesPage:
`
var user = await _userManager.FindByEmailAsync(User.Identity.Name);
await _userManager.AddToRoleAsync(user, "Admin");
await _context.SaveChangesAsync();
`
После запуска приложения нужно удалить эти строчки, перезапустить приложение и перезайти в аккаунт.
Если роль администратора отсуствует, ее можно создать:
`
await _roleManager.CreateAsync(role);
await _context.SaveChangesAsync();
`
