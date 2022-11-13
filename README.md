
### Deploy Consul container

```sh
# get image
docker pull consul

# add node to consul agent server
docker run -d --name=consul_node1 -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt": true,"acl": {"enabled": true,"default_policy": "deny","down_policy": "extend-cache","tokens": {"master": "token_value"}}}' consul agent -server -node=node1 -bootstrap-expect=2

# add node to consul agent server
docker run -d --name=consul_node2 -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt": true,"acl": {"enabled": true,"default_policy": "deny","down_policy": "extend-cache","tokens": {"master": "token_value"}}}' consul agent -server -node=node2 -bootstrap-expect=2 -join 172.17.0.2

# add node to consul agent client
docker run -d --name=consul_node3  -p 8500:8500 -e 'CONSUL_LOCAL_CONFIG={"skip_leave_on_interrupt": true,"acl": {"enabled": true,"default_policy": "deny","down_policy": "extend-cache","tokens": {"master": "token_value"}}}' consul agent -ui -node=node3 -client=0.0.0.0 -join 172.17.0.2
```

### Deploy WebApi 

```sh
# build image
docker build -t testwebapi .  

# run api container link consul container
docker run -d -p 8000:80 --name testwebapi testwebapi:latest  -join 172.17.0.2 --lin

# get container api ip
docker exec -it testwebapi ifconfig

```

### Discovery services

1. entry consul container

```sh
# entry consul
docker exec -it consul_node1 sh


# cd /consul/config ,create connect services
vi services.json

```

2. Setting container consul ip

```json
{
  "services": [
    {
      "id": "webapi1",
      "name": "webapi1",
      "tags": [
        "primary"
      ],
      "address": "172.17.0.3",
      "port": 80,
      "checks": [
        {
          "http": "http://172.17.0.3:80/api/healthz",
          "tls_skip_verify": false,
          "method": "Get",
          "interval": "10s",
          "timeout": "1s"
        }
      ],
      "token": "token_value"
    }
  ]
}
```

3. reload consul
```sh
# reload consul
consul reload -token=token_value
```