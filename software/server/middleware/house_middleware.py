class HouseMiddleware(object):
    """Serves the houses
    
    This class should accept commands, interpret them
    and return response to house request...
    
    """

    def __init__(self):
        self.houses = {} 
        

    def end_connection(self, socket):
        if socket in self.houses:
            print("Unregistered house with username " + self.houses[socket].username)
            del self.houses[socket]
        socket.transport.loseConnection()

    def command_error(self, err_msg):
        return ("ERROR:" + err_msg).encode("utf-8")

    def command_ok(self, ok_note):
        return ("OK:" + ok_note).encode("utf-8")

    def command_custom(self, cmd):
        return cmd.encode('utf-8')

    def rtrn(self, resp, disconnect_client = False):
        return resp , disconnect_client

    def parse_command(self, socket, data):
        data = data.decode('utf-8')
        parts = data.split(':')
        parts[0] = parts[0].lower()
        if(parts[0] == 'login'):
            if(len(parts) == 3):
                return self.login(socket, *parts[1::])
        
        if parts[0] == "ping":
            return self.pong()


        return False


    def login(self, socket,  username, password):
        if socket in self.houses:
            return self.rtrn(self.command_ok("Already logged in"))

        for _ , house in self.houses.items():
            if house.username == username:
                return self.rtrn(self.command_error("House with that username alrady existst"), True)

        self.houses[socket] = House(username, password)
        print("added house")
        return self.rtrn(self.command_ok("Logged in successfully"))

    def pong(self):
        return self.rtrn(False);

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