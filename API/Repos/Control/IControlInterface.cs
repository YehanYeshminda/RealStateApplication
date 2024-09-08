using API.Models;

namespace API.Repos.Control;

public interface IControlInterface
{
    List<Tblcontrol> GetAllControls();
    Tblcontrol GetControlTopOne();
    int UpdateControl(Tblcontrol tblcontrol);
}