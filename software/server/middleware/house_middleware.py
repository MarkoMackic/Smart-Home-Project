
from middleware import socket_parser


class HouseMiddleware(object):

    """Serves the houses

    This class is responsable for managing house
    objects and devices.

    """

    def __init__(self):
        self.houses = {} 
        
    def end_house_connection(self, socket):
        if socket in self.houses:
            print("Unregistered house with username " + self.houses[socket].username)
            del self.houses[socket]
        socket.transport.loseConnection()

    def parse_house_command(self, socket, data):
        return socket_parser.parse(self, socket, data)
    
    def add_house(self, socket, username, password):
        self.houses[socket] = House(username, password)
        return True

    def refresh_devices(self, socket, device_list):
        if socket in self.houses:
            self.populate_device_list(self.houses[socket], device_list)
            return True
        else:
            return False

    def populate_device_list(self, device_list):
        pass
    ''' Websocket parser '''    



class House(object):
    """House implementation

    Handles processing(parsing and operations) on single house
    """
    def __init__(self, username, password):
        self.username = username
        self.password = password
        self.devices = []

    def add_devices(self, devices):
        pass



class Device(object):
    """Abstract implementation of device"""
    def __init__(self, dname, dtype, dstate):
        self.name = dname
        self.type = dtype
        self.state = dstate


if __name__ == "__main__":
    print("Used as extention");
    exit()