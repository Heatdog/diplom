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
                    Msg = "Отдел не был обноружен";
                    break;
                case Errors.InvalidEmailAddress:
                    Msg = "Email уже используется";
                    break;
                case Errors.SaveDbError:
                    Msg = "Ошибка базы данных!";
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
}
}
