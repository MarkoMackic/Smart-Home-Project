import os
import sys
sys.path.insert(0, os.path.abspath(os.path.dirname(__file__)))

from twisted.internet.protocol import Factory
from twisted.protocols.basic import LineReceiver
from twisted.internet import reactor
from autobahn.twisted.websocket import WebSocketServerProtocol, \
    WebSocketServerFactory

from middleware.house_middleware import *

class HouseProtocol(LineReceiver):

    def __init__(self):  
        self.state = None

    def connectionMade(self):
        #self.sendLine("hello".encode('utf-8'));
        self.sendLine(b"V2_SMART_HOME_AUTOMATION")
        print("client connected")

    def connectionLost(self, reason):
        houseMiddleware.end_house_connection(self)
        print("Client lost")

    def lineReceived(self, line):
        # print("Line recieved ->  " + line.decode('utf-8'))
        response , disconnect_client = houseMiddleware.parse_house_command(self, line)
        if response != False:
            self.sendLine(response)
            # print("Responded with -> " + response.decode('utf-8'))
        
        if(disconnect_client):
            self.transport.loseConnection()



class HouseFactory(Factory):

    def buildProtocol(self, addr):
        return HouseProtocol()

class WebsocketFactory(WebSocketServerFactory):

    def __init__(self, addr):
        self.protocol = WebsocketProtocol
        super().__init__(addr)


class WebsocketProtocol(WebSocketServerProtocol):

    def onConnect(self, request):
        print("Client connecting: {0}".format(request.peer))

    def onOpen(self):
        print("WebSocket connection open.")

    def onMessage(self, payload, isBinary):
        if isBinary:
            print("Binary message received: {0} bytes".format(len(payload)))
        else:
            print("Text message received: {0}".format(payload.decode('utf8')))

        # echo back message verbatim
        self.sendMessage(payload, isBinary)

    def onClose(self, wasClean, code, reason):
        print("WebSocket connection closed: {0}".format(reason))

if __name__ == "__main__":
    
    houseMiddleware  = HouseMiddleware() 
    reactor.listenTCP(8123, HouseFactory())
    reactor.listenTCP(9000, WebsocketFactory(u"ws://127.0.0.1:9000"))

    reactor.run()






