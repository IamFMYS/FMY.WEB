﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMY.WCF.Test.Contract;
using FMY.WEB.Model;
using System.ServiceModel;
using System.Transactions;
using System.Diagnostics;
using System.Security.Principal;
using System.Security.Permissions;

namespace FMY.WCF.Test.Services
{
    //[ServiceBehavior(
    //   TransactionIsolationLevel = IsolationLevel.Serializable,
    //   TransactionTimeout = "00:00:30",
    //   InstanceContextMode = InstanceContextMode.PerSession,
    //   TransactionAutoCompleteOnSessionClose = true)]
    //[ServiceBehavior(IncludeExceptionDetailInFaults = true, TransactionTimeout = "00:30:00", InstanceContextMode = InstanceContextMode.PerCall)]
    //[DeliveryRequirements(RequireOrderedDelivery = true)]

    public class CalculatorService : ICalculator
    {
        [OperationBehavior(TransactionScopeRequired = true)]
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrators", Name = "FMY-PC\\FMY")]
        public int Add(int i, int j)
        {
            //身份标识接口：IIdentity 
            //三种实现： 1 WindowsIdentity 2 GenericIdentity  3 x509Identity
            //WindowsIdentity表示Windows账户
            //GenericIdentity通用型
            //1.2这两种如果身份标识名为空字符串，则验证失败；不为空，则验证通过。
            //x509Identity 代表X509证书验证通过的身份标识。x509Identity的身份表示总是验证通过的。
            Trace.Assert(ServiceSecurityContext.Current != null);
            Trace.Assert(OperationContext.Current.ServiceSecurityContext != null);
            Trace.Assert(ServiceSecurityContext.Current == OperationContext.Current.ServiceSecurityContext);
            IPrincipal principal = System.Threading.Thread.CurrentPrincipal;
            string userName = principal.Identity.Name;
            bool isAuthenticated = principal.Identity.IsAuthenticated;
            bool isInrole = principal.IsInRole(userName);
            isInrole = ((WindowsPrincipal)principal).IsInRole(WindowsBuiltInRole.Administrator);
            return i + j;
        }

        //[OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = false)]
        //[OperationBehavior(TransactionScopeRequired = true)]//, TransactionAutoComplete = true)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public int InserRegistEmail(UserRegistEmail userRegistEmailModel)
        {
            FMY.WEB.BLL.UserRegistEmailService userRegistEmailServie = new WEB.BLL.UserRegistEmailService(null);
            userRegistEmailServie.addEmailRecrd(new UserRegistEmail()
            {
                SendTime = DateTime.Now,
                Status = 1,
                UserId = 2,
                ValidateCode = "testcode"
            });
            return 1;
        }

        //[OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = false)]
        //[OperationBehavior(TransactionScopeRequired = true)]//, TransactionAutoComplete = true)]
        [OperationBehavior(TransactionScopeRequired = true)]
        public int InsertUser(User user)
        {
            //using (TransactionScope ts = new TransactionScope())
            //{
            if (string.IsNullOrEmpty(user.PassWord))
                user.PassWord = "123456";
            FMY.WEB.BLL.UserService userServie = new WEB.BLL.UserService();

            try
            {
                userServie.AddUser(user);
                System.Threading.Thread.Sleep(1000000);
                throw new Exception();
                //user.Name = user.Name + "_";

                //userServie.AddUser(user);
            }
            catch (Exception)
            {
                //System.Transactions.Transaction.Current.Rollback();
                throw;
            }
            //    ts.Complete();
            //}
            return 1;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public int InsertUserNoTrans(User user)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                FMY.WEB.BLL.UserService userServie = new WEB.BLL.UserService();
                userServie.AddUser(user);
                ts.Complete();
                return 1;
            }
        }
    }
}
