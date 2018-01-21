"""
 Used to serve house clients.

TODO : Maybe this rtrn(command_[ok/error/custom]) is unnecessery.
"""
import json


def command_error(module, err_code, err_msg):
    ''' Client should have defined err_codes to know 
    what should it do. I should write these somewhere
    to specify protocol exactly.
    '''
    if err_code < 1:
        return False
    return ("%s:%s:%s"%(module, err_code, err_msg)).encode("utf-8")


def command_ok(module, ok_note):
    return ("%s:0:%s"%(module, ok_note)).encode("utf-8")


def command_custom(cmd):
    return cmd.encode('utf-8')


def rtrn(resp, disconnect_client=False):
    return resp, disconnect_client


def parse(house_middleware, socket, data):
    data = data.decode('utf-8')
    parts = data.split(':', 1)
    parts[0] = parts[0].lower()
    if(parts[0] == 'login'):   # login:username:password
        if(len(parts) == 2):
            return login(house_middleware, socket, *parts[1].split(":"))

    if parts[0] == "ping":   # ping < NAT table keeping
        return pong(house_middleware)

    if parts[0] == "sds":   # sds : json ( devices )
        if len(parts) == 2:
            return set_dev_states(house_middleware, socket, parts[1])
    return False, True


def login(house_middleware, socket,  username, password):

    if socket in house_middleware.houses:
        return rtrn(command_ok("LOGIN","Already logged in"))

    for _, house in house_middleware.houses.items():
        if house.username == username:
            return rtrn(command_error("LOGIN", 1, "House with that username alrady exists"), True)

    if len(username) < 4 or len(password) < 6:
        return rtrn(command_error("LOGIN", 2, "Username or password too short"))

    # if you want to have users in db, you should implement it here

    success = house_middleware.add_house(socket, username, password)
    if success:
        return rtrn(command_ok("LOGIN", "Logged in successfully"))
    else:
        return rtrn(command_error("LOGIN", 3, "Something went wrong"), True)


def pong(house_middleware):
    return rtrn(False)


def set_dev_states(house_middleware, socket, devices_json):
    try:
        data = json.loads(devices_json)
        success = house_middleware.refresh_devices(socket, data)
        if success:
            return rtrn(command_ok("SDS", "Cool. Devices refreshed."))
        else:
            return rtrn(command_error("SDS", 1, "Not cool. Something went wrong."), True)
    except json.JSONDecodeError:
        return rtrn(command_error("SDS", 2, "Invalid JSON data"), True)
