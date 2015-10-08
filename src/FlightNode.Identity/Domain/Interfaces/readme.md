This directory is basically for interfaces to I/O systems, which can be used by domain logic but 
whose concrete implementations are not part of the domain. Instead the concrete implementations
are part of the Infrastructure area.

Domain logic classes may have interfaces as well, for dependency injection purposes. Those interfaces
should remain in the Logic folder.