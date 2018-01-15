from twisted.internet.protocol import Factory
from twisted.protocols.basic import LineReceiver
from twisted.internet import reactor
from autobahn.twisted.websocket import WebSocketServerProtocol, \
    WebSocketServerFactory




class HouseProtocol(LineReceiver):

    def __init__(self, users):
        self.state = None

    def connectionMade(self):
        pass

    def connectionLost(self, reason):
        pass

    def lineReceived(self, line):
        pass



class HouseFactory(Factory):

    def buildProtocol(self, addr):
        return SocketProtocol()

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

reactor.listenTCP(9000, WebsocketFactory(u"ws://127.0.0.1:9000"))
reactor.listenTCP(8123, HouseFactory())
reactor.run()





