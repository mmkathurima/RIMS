using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace CSUST.Data;

[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
public class TSmartProgressBarDesigner : ControlDesigner
{
	private DesignerActionListCollection m_ActionLists;

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			m_ActionLists ??= new DesignerActionListCollection
			{
                new TSmartProgressBarDesignerActionList(base.Component)
            };
			return m_ActionLists;
		}
	}
}
