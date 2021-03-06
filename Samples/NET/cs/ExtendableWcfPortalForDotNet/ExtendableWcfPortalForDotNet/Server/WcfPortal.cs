﻿using System;
using Csla.Server.Hosts;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Csla.Serialization;
using System.IO;
using System.Security.Principal;
using Csla.Core;
using Csla.Server;

namespace ExtendableWcfPortalForDotNet.Server
{
  /// <summary>
  /// Exposes server-side DataPortal functionality
  /// through WCF.
  /// </summary>
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
  [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
  public class WcfPortal : IExtendableWcfPortalForDotNet
  {
    #region IWcfPortal Members

    /// <summary>
    /// Create a new business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    public WcfResponse Create(CriteriaRequest request)
    {
      Csla.Server.DataPortal portal = new Csla.Server.DataPortal();
      Exception error = null;
      DataPortalResult  result = null;
      WcfResponse response = null;
      ISerializationFormatter formatter = SerializationFormatterFactory.GetFormatter();
      try
      {
        request = ConvertRequest(request);
        DataPortalContext context = new DataPortalContext(
            formatter.Deserialize(request.Principal) as IPrincipal,
            true,
            request.ClientCulture,
            request.ClientUICulture,
            formatter.Deserialize(request.ClientContext) as ContextDictionary,
            formatter.Deserialize(request.GlobalContext) as ContextDictionary);
        result = portal.Create(Type.GetType(request.TypeName), formatter.Deserialize(request.CriteriaData), context);
        response = new WcfResponse(
            formatter.Serialize(result.ReturnObject),
            formatter.Serialize(error),
            formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      catch (Exception ex)
      {
        error = ex;
        response = new WcfResponse(
           null,
           formatter.Serialize(error),
           formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      return ConvertResponse(response);
    }

    /// <summary>
    /// Get an existing business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    public WcfResponse Fetch(CriteriaRequest request)
    {
      Csla.Server.DataPortal portal = new Csla.Server.DataPortal();
      Exception error = null;
      DataPortalResult result = null;
      WcfResponse response = null;
      ISerializationFormatter formatter = SerializationFormatterFactory.GetFormatter();
      try
      {
        request = ConvertRequest(request);
        DataPortalContext context = new DataPortalContext(
            formatter.Deserialize(request.Principal) as IPrincipal,
            true,
            request.ClientCulture,
            request.ClientUICulture,
            formatter.Deserialize(request.ClientContext) as ContextDictionary,
            formatter.Deserialize(request.GlobalContext) as ContextDictionary);
        result = portal.Fetch(Type.GetType(request.TypeName), formatter.Deserialize(request.CriteriaData), context);
        response = new WcfResponse(
            formatter.Serialize(result.ReturnObject),
            formatter.Serialize(error),
            formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      catch (Exception ex)
      {
        error = ex;
        response = new WcfResponse(
           null,
           formatter.Serialize(error),
           formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      return ConvertResponse(response);
    }

    /// <summary>
    /// Update a business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    public WcfResponse Update(UpdateRequest request)
    {
      Csla.Server.DataPortal portal = new Csla.Server.DataPortal();
      Exception error = null;
      DataPortalResult result = null;
      WcfResponse response = null;
      ISerializationFormatter formatter = SerializationFormatterFactory.GetFormatter();
      try
      {
        request = ConvertRequest(request);
        DataPortalContext context = new DataPortalContext(
            formatter.Deserialize(request.Principal) as IPrincipal,
            true,
            request.ClientCulture,
            request.ClientUICulture,
            formatter.Deserialize(request.ClientContext) as ContextDictionary,
            formatter.Deserialize(request.GlobalContext) as ContextDictionary);
        result = portal.Update(formatter.Deserialize(request.ObjectData), context);
        response = new WcfResponse(
            formatter.Serialize(result.ReturnObject),
            formatter.Serialize(error),
            formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      catch (Exception ex)
      {
        error = ex;
        response = new WcfResponse(
           null,
           formatter.Serialize(error),
           formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      return ConvertResponse(response);
    }

    /// <summary>
    /// Delete a business object.
    /// </summary>
    /// <param name="request">The request parameter object.</param>
    [OperationBehavior(Impersonation = ImpersonationOption.Allowed)]
    public WcfResponse Delete(CriteriaRequest request)
    {
      Csla.Server.DataPortal portal = new Csla.Server.DataPortal();
      Exception error = null;
      DataPortalResult result = null;
      WcfResponse response = null;
      ISerializationFormatter formatter = SerializationFormatterFactory.GetFormatter();
      try
      {
        request = ConvertRequest(request);
        DataPortalContext context = new DataPortalContext(
            formatter.Deserialize(request.Principal) as IPrincipal,
            true,
            request.ClientCulture,
            request.ClientUICulture,
            formatter.Deserialize(request.ClientContext) as ContextDictionary,
            formatter.Deserialize(request.GlobalContext) as ContextDictionary);
        result = portal.Delete(Type.GetType(request.TypeName), formatter.Deserialize(request.CriteriaData), context);
        response = new WcfResponse(
            formatter.Serialize(result.ReturnObject),
            formatter.Serialize(error),
            formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      catch (Exception ex)
      {
        error = ex;
        response = new WcfResponse(
            null,
            formatter.Serialize(error),
            formatter.Serialize(Csla.ApplicationContext.GlobalContext));
      }
      return ConvertResponse(response);
    }

    #endregion

    protected virtual WcfResponse ConvertResponse(WcfResponse response)
    {
      return response;
    }

    protected virtual CriteriaRequest ConvertRequest(CriteriaRequest request)
    {
      return request;
    }
    protected virtual UpdateRequest ConvertRequest(UpdateRequest request)
    {
      return request;
    }
  }
}
