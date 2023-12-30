namespace Electronic_document_management.Models
{

    public class ErrorMessage
    {
        public string? Msg { get; set; }
        public ErrorMessage(Errors err)
        {
            switch (err)
            {
                case Errors.InvalidArguments:
                    Msg = "Неверный логин или пароль";
                    break;
                case Errors.InvalidUser:
                    Msg = "Логин уже используется";
                    break;
                case Errors.InvalidDepartment:
                    Msg = "Отдел уже существует";
                    break;
                case Errors.InvalidEmailAddress:
                    Msg = "Email уже используется";
                    break;
                case Errors.SaveDbError:
                    Msg = "Ошибка базы данных!";
                    break;
                case Errors.NotVerified:
                    Msg = "Пользователь проходит проверку учётных данных";
                    break;
                case Errors.EmptyValue:
                    Msg = "Пустое поле";
                    break;
                case Errors.IncorrectEmail:
                    Msg = "Некорректный email";
                    break;
                case Errors.RepeatPassword:
                    Msg = "Пароли не совпадают!";
                    break;
            }
        }
}
public enum Errors
{
    None,
    InvalidUser,
    InvalidArguments,
    InvalidDepartment,
    InvalidEmailAddress,
    SaveDbError,
    NotVerified,
    EmptyValue,
    IncorrectEmail,
    RepeatPassword,
    IncorrectPassword
}
}
