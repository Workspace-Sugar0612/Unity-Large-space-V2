using System;
using System.Net;
using Mirror;
using Mirror.Discovery;
using Unity.VisualScripting;
using UnityEngine;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-discovery
    API Reference: https://mirror-networking.com/docs/api/Mirror.Discovery.NetworkDiscovery.html
*/

public class CNNetworkDiscovery : NetworkDiscoveryBase<ServerRequest, ServerResponse>
{
    private CNAutoClient cnAutoClient;
    private CNNetworkLauncher _luncher;

    #region Server

    /// <summary>
    /// Reply to the client to inform it of this server
    /// </summary>
    /// <remarks>
    /// Override if you wish to ignore server requests based on
    /// custom criteria such as language, full server game mode or difficulty
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    protected override void ProcessClientRequest(ServerRequest request, IPEndPoint endpoint)
    {
        base.ProcessClientRequest(request, endpoint);
    }

    /// <summary>
    /// Process the request from a client
    /// </summary>
    /// <remarks>
    /// Override if you wish to provide more information to the clients
    /// such as the name of the host player
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    /// <returns>A message containing information about this server</returns>
    protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint) 
    {
        // In this case we don't do anything with the request
        // but other discovery implementations might want to use the data
        // in there,  This way the client can ask for
        // specific game mode or something

        try
        {
            // this is an example reply message,  return your own 
            // to include whatever is relevant for your game 
            return new ServerResponse
            {
                serverId = ServerId,
                uri = transport.ServerUri()
            };
        }
        catch (NotImplementedException)
        {
            Debug.LogError($"Transport {transport} does not support network discovery");
            throw;
        }
    }

    #endregion

    #region Client

    /// <summary>
    /// Create a message that will be broadcasted on the network to discover servers
    /// </summary>
    /// <remarks>
    /// Override if you wish to include additional data in the discovery message
    /// such as desired game mode, language, difficulty, etc... </remarks>
    /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
    protected override ServerRequest GetRequest()
    {
        return new ServerRequest();
    }

    /// <summary>
    /// Process the answer from a server
    /// </summary>
    /// <remarks>
    /// A client receives a reply from a server, this method processes the
    /// reply and raises an event
    /// </remarks>
    /// <param name="response">Response that came from the server</param>
    /// <param name="endpoint">Address of the server that replied</param>
    protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint) 
    {
        // we received a message from the remote endpoint
        response.EndPoint = endpoint;

        // although we got a supposedly valid url, we may not be able to resolve
        // the provided host
        // However we know the real ip address of the server because we just
        // received a packet from it,  so use that as host.
        UriBuilder realUri = new UriBuilder(response.uri)
        {
            Host = response.EndPoint.Address.ToString()
        };
        response.uri = realUri.Uri;

        //OnServerFound.Invoke(response);
        // if (_luncher == null)
        // {
        //    _luncher = FindObjectOfType<CNNetworkLauncher>();
        //    _luncher.OnDiscoveredServer(response);
        // }

        if (cnAutoClient == null)
        {
            cnAutoClient = (CNAutoClient)FindObjectOfType(typeof(CNAutoClient));
            cnAutoClient.OnDiscoveredServer(response);
        }
    }

    #endregion
}
